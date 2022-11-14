using System.Reflection.Metadata;
using System;
using System.Linq;

static void Main(string[] args)
{
    using var db = new UserContext();

    // Note: This sample requires the database to be created before running.
    Console.WriteLine($"Database path: {db.DbPath}.");

    // Create
    Console.WriteLine("Inserting a new blog");
    db.Add(new User { Username = "asdfasdfasdf" });
    db.SaveChanges();

    // Read
    //Console.WriteLine("Querying for a blog");
    //var blog = db.Blogs
    //    .OrderBy(b => b.Id)
    //    .First();

    //// Update
    //Console.WriteLine("Updating the blog and adding a post");
    //blog.Url = "https://devblogs.microsoft.com/dotnet";
    //blog.Posts.Add(
    //    new Post { Title = "Hello World", Content = "I wrote an app using EF Core!" });
    //db.SaveChanges();

    //// Delete
    //Console.WriteLine("Delete the blog");
    //db.Remove(blog);
    //db.SaveChanges();
}