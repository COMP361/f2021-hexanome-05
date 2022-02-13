using System;

namespace Models
{
    public abstract class GamePhase : IUpdatable<GamePhase>
    {
        abstract public event EventHandler Updated;
        public event EventHandler Ended;
        abstract public bool Update(GamePhase update);

        /// <summary>
        /// Checks update compatibility.
        /// </summary>
        /// <param name="update"></param>
        /// <returns>true if update has the same subtype as this object<br/> false otherwise</returns>
        abstract public bool isCompatible(GamePhase update);

        /// <summary>
        /// Invoked before transitioning to a new GamePhase
        /// </summary>
        public void End() {
            Ended?.Invoke(this, EventArgs.Empty);
        }
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