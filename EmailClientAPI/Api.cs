namespace EmailClientAPI
{
    public static class Api
    {
        public static void ConfigureApi(this WebApplication app)
        {
            app.MapPut("/CreateEmail", CreateEmail);
            app.MapPut("/UpdateRecipient", UpdateRecipient);
            app.MapPost("SendEmail", SendEmail);
        }

        private static async Task<IResult> CreateEmail(EmailClient ec, string subject, string body, string recipient)
        {
            try
            {
                //await ec.CreateEmail(subject, body, recipient);
                return Results.Ok();
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
                //await ec.UpdateRecipient(recipient);
                return Results.Ok();
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
                //await ec.SendEmail();
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
    }
}
