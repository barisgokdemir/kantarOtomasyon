using KantarOtomasyon.Core;

namespace KantarOtomasyon.Tests;

public class SifreHasherTests
{
    [Fact]
    public void Dogru_sifre_dogrulanir()
    {
        var hash = SifreHasher.Hashle("gizli123");
        Assert.True(SifreHasher.Dogrula("gizli123", hash));
    }

    [Fact]
    public void Yanlis_sifre_reddedilir()
    {
        var hash = SifreHasher.Hashle("gizli123");
        Assert.False(SifreHasher.Dogrula("gizli124", hash));
    }

    [Fact]
    public void Ayni_sifre_her_seferinde_farkli_tuzla_hashlenir()
    {
        Assert.NotEqual(SifreHasher.Hashle("abc1"), SifreHasher.Hashle("abc1"));
    }

    [Fact]
    public void Hash_duz_metin_sifreyi_icermez()
    {
        Assert.DoesNotContain("gizli123", SifreHasher.Hashle("gizli123"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("147")]
    [InlineData("a.b")]
    [InlineData("x.AAAA.AAAA")]
    [InlineData("0.AAAA.AAAA")]
    [InlineData("1000.!!!.AAAA")]
    [InlineData("1000.AAAA.")]
    public void Bozuk_hash_false_doner(string hash)
    {
        Assert.False(SifreHasher.Dogrula("147", hash));
    }
}
