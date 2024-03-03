namespace DB_Enlace.Helpers
{
    public static class EmailBody
    {
        public static string EmailStringBody(string email, string emailToken)
        {
            return $@"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Restablecer Contraseña</title>
                    <style>
                        body {{
                            font-family: 'Arial', sans-serif;
                            padding: 20px;
                            text-align: center;
                        }}
                        .container {{
                            max-width: 600px;
                            margin: 0 auto;
                            background-color: #f5f5f5;
                            padding: 20px;
                            border-radius: 10px;
                            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                        }}
                        h2 {{
                            color: #333;
                        }}
                        p {{
                            color: #666;
                        }}
                        .cta-button {{
                            display: inline-block;
                            padding: 10px 20px;
                            margin-top: 20px;
                            background-color: #007bff;
                            color: #fff;
                            text-decoration: none;
                            border-radius: 5px;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h2>Restablecer Contraseña</h2>
                        <p>Has solicitado restablecer tu contraseña. Haz clic en el siguiente enlace para cambiar tu contraseña:</p>
                        <a href=""https://jolly-wave-0788d9610.4.azurestaticapps.net/reset?email={email}&code={emailToken}"">Restablecer Contraseña</a>
                        <p>Si no has solicitado este restablecimiento, puedes ignorar este correo de forma segura.<br><br>
                        Saludos cordiales.<br><br>
                        Ministerio infantil Enlace.</p>
                    </div>
                </body>
                </html>
            ";
        }
    }
}