// Patron creacional: Builder
using System.Text;
namespace Exchanger.Domain;
public class Token
{
    public string Value { get; }

    public Token(string value)
    {
        Value = value;
    }

    public class TokenBuilder
    {
        private StringBuilder tokenBuilder;

        public TokenBuilder()
        {
            tokenBuilder = new StringBuilder();
        }

        public TokenBuilder AddRandomChars(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                tokenBuilder.Append(chars[random.Next(chars.Length)]);
            }
            return this;
        }

        public Token Build()
        {
            return new Token(tokenBuilder.ToString());
        }
    }
}