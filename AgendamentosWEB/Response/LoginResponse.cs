﻿namespace AgendamentosWEB.Response
{
    public class LoginResponse
    {
        public bool Sucesso { get; set; }
        public string[]? Erros { get; set; }
        public string? Token { get; set; }
        public List<string>? Roles { get; set; }
    }
}