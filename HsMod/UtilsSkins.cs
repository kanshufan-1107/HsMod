using static HsMod.PluginConfig;

namespace HsMod
{
    public partial class Utils
    {

        public static class CheckInfo
        {
            public static bool IsCoin()
            {
                if (CacheCoin.Count == 0) CacheInfo.UpdateCoin();
                if (CacheCoin.Contains(skinCoin.Value)) return true;
                else return false;
            }
            public static bool IsCoin(string cardId)
            {
                int dbId = GameUtils.TranslateCardIdToDbId(cardId);
                if (CacheCoinCard.Count == 0) CacheInfo.UpdateCoinCard();
                if (CacheCoinCard.Contains(dbId)) return true;
                else return false;
            }
            public static bool IsBoard()
            {
                if (CacheGameBoard.Count == 0) CacheInfo.UpdateGameBoard();
                if (CacheGameBoard.Contains(skinBoard.Value)) return true;
                else return false;
            }
            public static bool IsBgsBoard()
            {
                if (CacheBgsBoard.Count == 0) CacheInfo.UpdateBgsBoard();
                if (CacheBgsBoard.Contains(skinBgsBoard.Value)) return true;
                else return false;
            }
            public static bool IsHero(int DbID, out Assets.CardHero.HeroType heroType)
            {
                if (CacheHeroes.Count == 0) CacheInfo.UpdateHeroes();
                if (CacheHeroes.ContainsKey(DbID))
                {
                    heroType = CacheHeroes[DbID];
                    return true;
                }
                if (DefLoader.Get()?.GetEntityDef(DbID)?.GetCardType() == TAG_CARDTYPE.HERO)
                {
                    heroType = Assets.CardHero.HeroType.UNKNOWN;
                    return true;
                }
                else
                {
                    heroType = Assets.CardHero.HeroType.UNKNOWN;
                    return false;
                }
            }
            public static bool IsCardBack()
            {
                if (CacheCardBack.Count == 0) CacheInfo.UpdateCardBack();
                if (CacheCardBack.Contains(skinCardBack.Value)) return true;
                else return false;
            }
            public static bool IsBgsFinisher()
            {
                if (CacheBgsFinisher.Count == 0) CacheInfo.UpdateBgsFinisher();
                if (CacheBgsFinisher.Contains(skinBgsFinisher.Value)) return true;
                else return false;
            }

            public static bool IsHero(string cardID, out Assets.CardHero.HeroType heroType)
            {
                if (CacheHeroes.Count == 0) CacheInfo.UpdateHeroes();
                int dbid = GameUtils.TranslateCardIdToDbId(cardID);
                if (CacheHeroes.ContainsKey(dbid))
                {
                    heroType = CacheHeroes[dbid];
                    return true;
                }
                else
                {
                    heroType = Assets.CardHero.HeroType.UNKNOWN;
                    return false;
                }
            }

            public static bool IsMercenarySkin(string cardID, out MercenarySkin skin)
            {
                if (CacheMercenarySkin.Count == 0) CacheInfo.UpdateMercenarySkin();
                int dbid = GameUtils.TranslateCardIdToDbId(cardID);

                foreach (var mercSkin in CacheMercenarySkin)
                {
                    if (mercSkin.Id.Contains(dbid))
                    {
                        skin = mercSkin;
                        return true;
                    }
                }
                skin = new MercenarySkin();
                return false;
            }

        }

        public static void UpdateHeroTag(string cardId)
        {
            if (!Utils.CheckInfo.IsHero(cardId, out _))
            {
                return;
            }
            var defLoader = DefLoader.Get();
            if (defLoader == null)
            {
                return;
            }
            //if (!defLoader.HasLoadedEntityDefs())
            //{
            //    defLoader.LoadAllEntityDefs();
            //}
            EntityDef entityDef = defLoader.GetEntityDef(cardId);

            if (entityDef?.GetTag(GAME_TAG.EMOTECHARACTER) > 0)
            {
                GameState gameState = GameState.Get();
                if (gameState == null)
                {
                    return;
                }
                int entityId = gameState.GetPlayerBySide(Player.Side.FRIENDLY).GetEntityId();
                int tag = gameState.GetEntity(entityId).GetTag(GAME_TAG.HERO_ENTITY);
                gameState.GetEntity(tag).SetTag(GAME_TAG.EMOTECHARACTER, entityDef.GetTag(GAME_TAG.EMOTECHARACTER));
            }
            if (entityDef?.GetTag(GAME_TAG.CORNER_REPLACEMENT_TYPE) > 0)
            {
                GameState gameState2 = GameState.Get();
                if (gameState2 == null)
                {
                    return;
                }
                int entityId2 = gameState2.GetPlayerBySide(Player.Side.FRIENDLY).GetEntityId();
                gameState2.GetEntity(entityId2).SetTag(GAME_TAG.CORNER_REPLACEMENT_TYPE, entityDef.GetTag(GAME_TAG.CORNER_REPLACEMENT_TYPE));
                new CornerSpellReplacementManager(false).UpdateCornerReplacements(CornerReplacementSpellType.NONE, CornerReplacementSpellType.NONE);
            }

        }
    }

}
