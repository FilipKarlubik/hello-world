using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tribes.Tests.BattlesTest
{
    [Serializable]
    [Collection("Serialize")]
    public class BattleModelTest
    {
        [Fact]
        public void TestBattleModelCreation()
        {
            var Battle = new Battle(1,2,Outcome.Defeat);
            Assert.NotNull(Battle);
            Assert.True(Battle.Fought_at < DateTime.Now);
            Assert.True(Battle.Fought_at > DateTime.Now.AddMinutes(-1));
            Assert.True(Battle is Battle);
            Assert.Equal(1, (int)Battle.Outcome);
            Assert.Equal(1, Battle.AttackerId);
            Assert.Equal(2, Battle.DefenderId);
        }
    }
}
