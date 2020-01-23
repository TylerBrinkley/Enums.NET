using System;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;

namespace EnumsNET.Tests.Issues
{
    [TestFixture]
    public class Issue34
    {
        [Test]
        public void GetFlags_SuccessfullyEnumerates_WhenUsingLargeUnsignedEnumValue()
        {
            CollectionAssert.AreEqual(new[] { Wayspot.Pokestop, Wayspot.Gym, Wayspot.ExRaidGym, Wayspot.Other }, Wayspot.Wayspots.GetFlags());
        }

        [Flags]
        public enum Wayspot : byte
        {
            [Display(Name = "Pokéstop", GroupName = "static_assets/png/btn_pokestop.png", Order = -10)]
            Pokestop = 1,
            [Display(Name = "Gym", GroupName = "static_assets/png/gymLogo.png", Order = -50)]
            Gym = 2,
            [Display(Name = "EX-Raid Gym", GroupName = "static_assets/png/gymLogo.png", Order = -100)]
            ExRaidGym = 4,
            [Display(Name = "In Queue", GroupName = "static_assets/png/btn_help_nooutline.png")]
            NominationReview = 8,
            [Display(Name = "Rejected", GroupName = "static_assets/png/btn_close_normal_dark.png")]
            NominationRejected = 16,
            [Display(Name = "Accepted", GroupName = "static_assets/png/btn_ok_normal_dark.png")]
            NominationAccepted = 32,
            Other = 128,
            [Display(Name = "Wayspots", Order = 1)]
            Wayspots = Pokestop | Gym | ExRaidGym | Other,
            [Display(Name = "Nominations", Order = 2)]
            Nominations = NominationReview | NominationRejected | NominationAccepted
        }
    }
}
