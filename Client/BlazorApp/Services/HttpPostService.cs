using System.Text.Json;
using APIContracts;

namespace BlazorApp.Services;

public class HttpPostService : IPostService
{
    private readonly HttpClient client;

    public HttpPostService(HttpClient client)
    {
        this.client = client;
    }

    public async Task AddPostAsync (CreatePostDto request)
    {
        HttpResponseMessage httpResponse = await client.PostAsJsonAsync($"https://localhost:7207/Posts", request);

        if (!httpResponse.IsSuccessStatusCode)
        {
            string response = await httpResponse.Content.ReadAsStringAsync();
            throw new Exception(response);
        }
    }

    public async Task UpdatePostAsync(int id, UpdatePostDto request)
    {
        HttpResponseMessage httpResponse = await client.PutAsJsonAsync($"https://localhost:7207/Posts/{id}", request);
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
    }

    public async Task<PostDto> GetPostAsync(int id)
    {
        HttpResponseMessage httpResponse = await client.GetAsync($"https://localhost:7207/Posts/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<PostDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<IEnumerable<PostDto>> GetPostsAsync()
    {
        HttpResponseMessage httpResponse = await client.GetAsync("https://localhost:7207/Posts");
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        
        return JsonSerializer.Deserialize<IEnumerable<PostDto>>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task DeletePostAsync(int id)
    {
        HttpResponseMessage httpResponse = await client.DeleteAsync($"https://localhost:7207/Posts/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
    }

    public async Task<PostWithCommentsDTO> GetPostByIdAsync(int id)
    {
        HttpResponseMessage response = await client.GetAsync($"https://localhost:7207/Posts/{id}");
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        Console.WriteLine("API Response: " + content);

        return JsonSerializer.Deserialize<PostWithCommentsDTO>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<List<CommentDto>> GetCommentsAsync(int postId)
    {
        try
        {
            HttpResponseMessage response = await client.GetAsync($"https://localhost:7207/Posts/{postId}/Comments");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new List<CommentDto>();
            }
            response.EnsureSuccessStatusCode();
            var comments = await response.Content.ReadFromJsonAsync<List<CommentDto>>();
            
            foreach (var comment in comments)
            {
                Console.WriteLine($"Comment ID: {comment.Id}, Body: {comment.Body}, UserId: {comment.UserId}, PostId: {comment.PostId}");
            }

            return comments;
        }
        catch (HttpRequestException ex)
        {
            throw new Exception("Error fetching comments: " + ex.Message, ex);
        }
    }

    public async Task<CreateCommentDto> AddCommentAsync(CreateCommentDto dto)
    {
        try
        {
            HttpResponseMessage response = await client.PostAsJsonAsync($"https://localhost:7207/Comments", dto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CreateCommentDto>();
        }
        catch (HttpRequestException ex)
        {
            throw new Exception("Error adding comment: " + ex.Message, ex);
        }
    }

    
}