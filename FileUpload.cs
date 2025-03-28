using RestSharp;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

class FileUploadApi
{
    static void Main()
    {
        var client = new RestClient("https://petstore.swagger.io/v2");

        // Create a new pet
        int petId = 987654; // Unique pet ID
        var createRequest = new RestRequest("/pet", Method.Post);
        createRequest.AddJsonBody(new { id = petId, name = "Fluffy", status = "available" });

        var createResponse = client.Execute(createRequest);
        Debug.WriteLine($"Pet Created: {createResponse.Content}");

        // Add delay to ensure pet is created before proceeding
        Thread.Sleep(2000);

        // Upload an image
        string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Mountains.jpg");

        if (!File.Exists(imagePath))
        {
            Debug.WriteLine($"Error: File not found at {imagePath}");
            return;
        }

        var uploadRequest = new RestRequest($"/pet/{petId}/uploadImage", Method.Post);
        uploadRequest.AddFile("file", imagePath);

        var uploadResponse = client.Execute(uploadRequest);
        Debug.WriteLine($"Image Upload Response: {uploadResponse.Content}");

        // Verify the image was uploaded successfully
        if (uploadResponse.StatusCode == System.Net.HttpStatusCode.OK)
        {
            Debug.WriteLine("File uploaded successfully!");
        }
        else
        {
            Debug.WriteLine("File upload failed!");
        }
    }
}
