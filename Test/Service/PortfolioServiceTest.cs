using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using BankManager.Common.Model.Configuration;
using BankManager.Core.Model.Porfolio;
using BankManager.Core.Provider.Impl;
using BankManager.Core.Service;
using BankManager.Core.Service.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankManager.Test.Service
{
    [TestClass]
    public class PortfolioServiceTest : ContainerBasedTestBase
    {
        public IPortfolioService PortfolioService { get; set; }

        private PortfolioGroupModel groupModel1 =
            new PortfolioGroupModel
            {
                Name = "Test MSCI World",
                AssignedIsins = new[] { "IE12345678", "DE98765432" },
                LowerThresholdPercentage = 12m,
                TargetPercentage = 25m,
                UpperThresholdPercentage = 37m
            };

        [TestInitialize]
        public void TestInitialize()
        {
            base.TestInitialize(useTestDatabase: true);
            PortfolioService = Container.Resolve<IPortfolioService>();
        }

        [TestMethod]
        public void TestCreatePortfolioGroup()
        {
            var createdGroup = PortfolioService.CreateEditPortfolioGroup(groupModel1);
            var returnedGroup = PortfolioService.GetPortfolioGroup(createdGroup.Id);
            Assert.IsNotNull(returnedGroup);
            Assert.AreEqual(createdGroup.Name, returnedGroup.Name);
        }

        [TestCleanup]
        public override void TestCleanup()
        {
            base.TestCleanup();
        }
    }
}
