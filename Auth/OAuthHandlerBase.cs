﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth
{
    public abstract class OAuthHandlerBase
    {
        protected readonly IHttpClientFactory _httpClientFactory;
        protected readonly ILogger _logger;

        protected readonly GithubOauthOptions _clientOptions;

        protected OAuthHandlerBase(IHttpClientFactory httpClientFactory, ILogger logger, GithubOauthOptions options)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _clientOptions = options;
        }

        public abstract Task<string> ExchangeToken(TokenRequest request);
    }
}
