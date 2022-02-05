
namespace Models
{
    public abstract class GamePhase : IUpdatable<GamePhase>
    {
        public enum Elfenland {
            DealTravelCards,
            DrawTransportationCounter,
            DrawAdditionalTransportationCounters,
            PlanTravelRoutes,
            MoveElfBoot,
            FinishRound
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
}