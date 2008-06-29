using Microsoft.MediaPlayer.Interop;
using NUnit.Framework;
using Rhino.Mocks;
using JukeBox;

namespace JukeBoxTest
{
    [TestFixture]
    public class UtilityTest
    {
        [Test]
        public void shouldReturnNullTrackFromWMPMediaWithNonAudioMediaType()
        {
            var mocks = new MockRepository();
            var item = mocks.StrictMock<IWMPMedia>();
            Expect.Call(item.getItemInfo("MediaType")).Return("nonaudio");
            mocks.ReplayAll();
            Assert.IsNull(Utility.CreateTrack(item));
            mocks.VerifyAll();
        }
    }
}
