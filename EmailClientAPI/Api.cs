namespace EmailClientAPI
{
    public static class Api
    {
        public static void ConfigureApi(this WebApplication app)
        {
            app.MapPut("/createemail", CreateEmail);
            app.MapPut("/updaterecipient", UpdateRecipient);
            app.MapPost("/sendemail", SendEmail);
        }

        private static async Task<IResult> CreateEmail(EmailClient ec, string subject, string body, string recipient)
        {
            try
            {
                String message = ec.CreateEmail(subject, body, recipient);
                return Results.Ok(message);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        private static async Task<IResult> UpdateRecipient(EmailClient ec, string recipient)
        {
            try
            {
                ec.UpdateRecipient(recipient);
                return Results.Ok(recipient);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        private static async Task<IResult> SendEmail(EmailClient ec)
        {
            try
            {
                String result = ec.SendEmail();
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
    }
}
