/*
    To maintain structure where input user data is converted to DTO we should have a login DTO
    as well. Contains the fields:
        - Email
        - Password
    There is no need for a mapper because as right now there is no User entity in the database.
*/

namespace LibraryManagementAPI.DTOs;

public class LoginRequestDTO
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
