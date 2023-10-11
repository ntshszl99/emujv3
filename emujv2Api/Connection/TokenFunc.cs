using System;
using System.Collections.Generic;
using System.Text;
using JWT;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;
using Newtonsoft.Json;

namespace ConnectionModule
{
    public class TokenFunc
    {
        private static readonly string TokenSecret = "N0S0upR4nc1dd";
        private string CurrentDateTime()
        {
            return DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");

        }
        private string CombineString(params Object[] array)
        {
            StringBuilder builder = new StringBuilder();
            foreach (Object obj in array)
            {
                builder.Append(obj);
            }
            return builder.ToString();
        }

        public String CreateToken(Dictionary<String, Object> payload)
        {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder Encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            String token = Encoder.Encode(payload, TokenFunc.TokenSecret);
            return token;
        }

        //public Cons_ValidateUser ValidateToken(String Token, ref string ErrMsg)
        public string ValidateToken(String Token, ref string ErrMsg)
        {
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
                IDateTimeProvider Provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, Provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder Decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);
                String json = Decoder.Decode(Token, TokenFunc.TokenSecret, true);
                //Cons_ValidateUser data = JsonConvert.DeserializeObject<Cons_ValidateUser>(json);
                ErrMsg = "";
                return json;
            }
            catch (TokenExpiredException exc) { ErrMsg = exc.Message.ToString(); return null; }
            catch (SignatureVerificationException exc) { ErrMsg = exc.Message.ToString(); return null; }
            catch (Exception exc) { ErrMsg = exc.Message.ToString(); return null; }
        }
    }
}
