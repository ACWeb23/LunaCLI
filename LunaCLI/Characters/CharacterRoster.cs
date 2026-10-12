namespace Zenless
{
    /// <summary>
    /// Builds one of every agent at the requested mindscape count.
    /// </summary>
    static class CharacterRoster
    {
        public static IEnumerable<Character> CreateAll(int mindscapes = 0)
        {
            yield return new AliceThymefield(mindscapes);
            yield return new AlexandrinaSebastiane(mindscapes);
            yield return new Aria(mindscapes);
            yield return new AsabaHarumasa(mindscapes);
            yield return new AstraYao(mindscapes);
            yield return new Banyue(mindscapes);
            yield return new BurniceWhite(mindscapes);
            yield return new CaesarKing(mindscapes);
            yield return new Cissia(mindscapes);
            yield return new ClaretFlint(mindscapes);
            yield return new Dialyn(mindscapes);
            yield return new EllenJoe(mindscapes);
            yield return new EvelynChevalier(mindscapes);
            yield return new GraceHoward(mindscapes);
            yield return new HoshimiMiyabi(mindscapes);
            yield return new HugoVlad(mindscapes);
            yield return new JaneDoe(mindscapes);
            yield return new JuFufu(mindscapes);
            yield return new KoledaBelobog(mindscapes);
            yield return new Lighter(mindscapes);
            yield return new LuciaElowen(mindscapes);
            yield return new NangongYu(mindscapes);
            yield return new NekomiyaMana(mindscapes);
            yield return new NormaHollowell(mindscapes);
            yield return new OrphieMagnussonAndMagus(mindscapes);
            yield return new Promeia(mindscapes);
            yield return new Qingyi(mindscapes);
            yield return new Remielle(mindscapes);
            yield return new RoxyIfritaPryce(mindscapes);
            yield return new Seed(mindscapes);
            yield return new SigridDeLAzur(mindscapes);
            yield return new Soldier0Anby(mindscapes);
            yield return new Soldier11(mindscapes);
            yield return new StarlightBillyKid(mindscapes);
            yield return new Sunna(mindscapes);
            yield return new Trigger(mindscapes);
            yield return new TsukishiroYanagi(mindscapes);
            yield return new UkinamiYuzuha(mindscapes);
            yield return new VelinaAirgid(mindscapes);
            yield return new VivianBanshee(mindscapes);
            yield return new VonLycaon(mindscapes);
            yield return new YeShunguang(mindscapes);
            yield return new YidhariMurphy(mindscapes);
            yield return new Yixuan(mindscapes);
            yield return new Zhao(mindscapes);
            yield return new ZhuYuan(mindscapes);
            yield return new Pyrois(mindscapes);
        }
    }
}