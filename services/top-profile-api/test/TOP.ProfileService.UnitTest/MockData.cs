using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TOP.ProfileService.Domain.Entities;
using TOP.ProfileService.Domain.Enums;
using TOP.ProfileService.Domain.ValueObject;
using TOP.ProfileService.Infra.Data.Context;

namespace TOP.ProfileService.UnitTest
{
    public class MockData
    {
        public AppDbContext DbCtx { get; private set; }

        public MockData()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

            DbCtx = new AppDbContext(options);

            DbCtx.UserProfiles.AddRange(GetUserProfiles());
            DbCtx.UserProfileDetails.AddRange(GetUserProfileDetails());
            DbCtx.SaveChanges();
        }

        private static List<UserProfile> GetUserProfiles()
        {
            var data = new List<UserProfile>(3)
            {
                new UserProfile("21DCE4AF-7DFF-4D11-8BD4-2EAC339282A4")
                {
                    FirstName = "Tester",
                    LastName = "1",
                    UserName = "teste.1",
                    Email = "teste.1@email.com",
                    PhoneNumber = "11 1111-1111",
                    BirthDate = new DateTime(1992, 5, 15),
                    AcademicLevel = AcademicLevel.Bachelor
                },
                new UserProfile("4050F49F-F5AE-4976-A80B-AD2E8BEF3E2E")
                {
                    FirstName = "Tester",
                    LastName = "2",
                    UserName = "teste.2",
                    Email = "teste.2@email.com",
                    PhoneNumber = "11 2222-2222",
                    BirthDate = new DateTime(1991, 2, 3),
                    AcademicLevel = AcademicLevel.Graduate
                },
                new UserProfile("7A5D61B8-72BC-4A3F-8E62-98F91B052248")
                {
                    FirstName = "Tester",
                    LastName = "3",
                    UserName = "teste.3",
                    Email = "teste.3@email.com",
                    PhoneNumber = "11 3333-3333",
                    BirthDate = new DateTime(1998, 10, 24),
                    AcademicLevel = AcademicLevel.HigherEducation
                }
            };
            return data;
        }

        private static List<UserProfileDetails> GetUserProfileDetails()
        {
            var data = new List<UserProfileDetails>(3)
            {
                new UserProfileDetails()
                {
                    UserId = Guid.Parse("21DCE4AF-7DFF-4D11-8BD4-2EAC339282A4"),
                    Address = new Address()
                    {
                        Street = "Rua teste 1, 123",
                        District = "Vila test",
                        City = "Tester City",
                        State = "State 1",
                        Cep = "01234-567"
                    },
                    Bio = "Bio tester 1",
                    ProfileImage = Encoding.UTF8.GetBytes("Profile Image 1")
                },
                new UserProfileDetails()
                {
                    UserId = Guid.Parse("4050F49F-F5AE-4976-A80B-AD2E8BEF3E2E"),
                    Address = new Address()
                    {
                        Street = "Rua teste 2, 456",
                        District = "Vila test",
                        City = "Tester City",
                        State = "State 2",
                        Cep = "98765-432"
                    },
                    Bio = "Bio tester 2",
                    ProfileImage = Encoding.UTF8.GetBytes("Profile Image 2")
                },
                new UserProfileDetails()
                {
                    UserId = Guid.Parse("7A5D61B8-72BC-4A3F-8E62-98F91B052248"),
                    Address = new Address()
                    {
                        Street = "Rua teste 3, 789",
                        District = "Vila test",
                        City = "Tester City",
                        State = "State 3",
                        Cep = "14725-839"
                    },
                    Bio = "Bio tester 3",
                    ProfileImage = Encoding.UTF8.GetBytes("Profile Image 3")
                },
            };
            return data;
        }
    }
}
