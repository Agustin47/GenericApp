namespace GenericWebApp.Models.Requests;

public record UmCreateUser(string Username, string Password, string Email, string Name, string LastName, string Role, string[] Permissions);