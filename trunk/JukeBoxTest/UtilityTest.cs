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

        [Test]
        public void shouldReturnTrackFromWMPMediaWithAudioMediaType()
        {
            var mocks = new MockRepository();
            var item = mocks.StrictMock<IWMPMedia>();

            Expect.Call(item.getItemInfo("MediaType")).Return("audio");
            Expect.Call(item.name).Return("trackname");
            Expect.Call(item.getItemInfo("WM/TrackNumber")).Return("1");
            Expect.Call(item.getItemInfo("Author")).Return("artist");
            Expect.Call(item.getItemInfo("WM/AlbumArtist")).Return("album artist");
            Expect.Call(item.getItemInfo("WM/AlbumTitle")).Return("album title");
            Expect.Call(item.duration).Return(60);
            Expect.Call(item.sourceURL).Return(@"c:\path\to\file.mp3");

            mocks.ReplayAll();
            Assert.IsNotNull(Utility.CreateTrack(item));
            mocks.VerifyAll();
        }
    }
}
