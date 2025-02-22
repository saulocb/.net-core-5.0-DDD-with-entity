﻿using $ext_projectname$.Common;
using LinqToTwitter.OAuth;

namespace $safeprojectname$
{
    public static class Setup
    {
        public static IAuthorizer Authorization(ApiSettings settings)
        {
            var auth = new SingleUserAuthorizer()
            {
                CredentialStore = new SingleUserInMemoryCredentialStore
                {
                    ConsumerKey = settings.Key,
                    ConsumerSecret = settings.Secret,
                    AccessTokenSecret = settings.TokenSecret,
                    AccessToken = settings.AccessToken
                },
            };

            return auth;
        }
    }
}
