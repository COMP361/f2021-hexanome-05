
namespace Models
{
    public abstract class GamePhase // : IUpdatable<GamePhase>
    {
        
    }

    public enum Elfenland {
            DealCardsAndCounters = 1,
            DrawAdditionalTransportationCounters = 2,
            PlanTravelRoutes = 3,
            MoveElfBoot = 4,
            FinishRound = 5
        }

        public enum Elfengold {
            DrawTravelCards,
            DistributeGoldCoins,
            DrawTokensAndCounters,
            Auction,
            PlanTravelRoutes,
            MoveElfBoot,
            FinishRound
        }
}