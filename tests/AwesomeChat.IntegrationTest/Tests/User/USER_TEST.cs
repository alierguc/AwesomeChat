using AwesomeChat.Domain.Entities;
using AwesomeChat.Persistence.Context;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AwesomeChat.IntegrationTest.Tests.User
{


    public class USER_TEST 
    {
        private Mock<ApplicationDbContext> mockContext = new Mock<ApplicationDbContext>();
        private Mock<DbSet<ApplicationUser>> mockSet = new Mock<DbSet<ApplicationUser>>();
        private ApplicationDbContext appDbContext;

        [Fact]
        public void findUserByUserName_TEST()
        {
          
            mockContext.Setup(m => m.Users).Returns(mockSet.Object);
            mockSet.Verify(m => m.Add(It.IsAny<ApplicationUser>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
           
        }
    }
}
