using Microsoft.EntityFrameworkCore;
using ProductManagement.Data;
using ProductManagement.Model;
using ProductManagement.Providers;

namespace ProductManagement.Business
{
    public class LoginProcess
    {
        public static async Task<Response> LoginMech(AuthModel data, bool checkNewPass = true)
        {
            Response apiResponse = new() { Status = (byte)StatusFlags.Failed };
            try
            {
                Connection connection = new Connection();
                if (connection != null && !connection.IsDeleted)
                {
                    if (connection.IsActive)
                    {
                        using DefaultContext defaultContext = new(connection);
                        Users user = await defaultContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == data.Username && u.Password == data.Password);

                        if (user == null) { apiResponse.Message = "Enter valid credentials"; }
                        else
                        {
                            user.Password = "";
                            apiResponse = new Response { Data = user, Status = (byte)StatusFlags.Success };
                        }
                    }
                    else { apiResponse.Message = "Connection deactivated"; }
                }
                else { apiResponse.Message = "Connection not found"; }
            }
            catch (Exception ex) { apiResponse.Status = (byte)StatusFlags.Failed; }
            return apiResponse;
        }
    }
}
