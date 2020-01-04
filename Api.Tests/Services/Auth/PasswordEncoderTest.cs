using System;
using System.Collections.Generic;
using System.Linq;
using Api.Services.Auth;
using Common.Configuration;
using Microsoft.Extensions.Options;
using Xunit;

namespace Api.Tests.Services.Auth
{
    public class PasswordEncoderTest
    {
        public static IEnumerable<object[]> PossiblePasswords
        {
            // ReSharper disable once UnusedMember.Global -- it is used by TestEncodeDecode method
            get
            {
                // Or this could read from a file. :)
                return new[]
                {
                    // upper case
                    new object[] {RandomString(5, "ABCDEFGHIJKLMNOPQRSTUVWXYZ")},
                    // upper case with numbers
                    new object[] {RandomString(10, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")},
                    // lower, upper cases and numbers
                    new object[] {RandomString(15, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890")},
                    // all printable characters
                    new object[]
                    {
                        RandomString(20, string.Concat(Enumerable.Range(0, char.MaxValue + 1)
                            .Select(i => (char) i)
                            .Where(c => !char.IsControl(c))))
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(PossiblePasswords))]
        public void TestEncodeDecode(string password)
        {
            var hashingOptions = new HashingOptions {Iterations = 1000};
            var service = new EncodingService(Options.Create(hashingOptions));

            var encoded = service.Encode(password);

            Assert.True(service.DoesHashCorrespondToPassword(password, encoded));
        }

        private static string RandomString(int length, string? alphabet = null)
        {
            var random = new Random();
            var chars = alphabet ?? "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}