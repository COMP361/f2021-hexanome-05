using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Models;

public class UpdateTests
{
    private void CreateModel() {
        // Board board = new Board();
        // towns.set("Al'Baran",new Town("Al'Baran",intializedGameData.colorsChosen.map(color => new TownPiece(color)),[]));
        // towns.set("Beata", new Town("Beata",intializedGameData.colorsChosen.map(color => new TownPiece(color)),[]));
        // towns.set("Elfenhold",new Town("Elfenhold",intializedGameData.colorsChosen.map(color => new TownPiece(color)),[]));
        // towns.set("Dag'Amura", new Town("Dag'Amura",intializedGameData.colorsChosen.map(color => new TownPiece(color)),[]));
        // towns.set("Erg'Eren", new Town("Erg'Eren",intializedGameData.colorsChosen.map(color => new TownPiece(color)),[]));
        // towns.set("Feodor", new Town("Feodor",intializedGameData.colorsChosen.map(color => new TownPiece(color)),[]));
        // towns.set("Grangor", new Town("Grangor",intializedGameData.colorsChosen.map(color => new TownPiece(color)),[]));
        // towns.set("Jaccaranda", new Town("Jaccaranda",intializedGameData.colorsChosen.map(color => new TownPiece(color)),[]));
        // towns.set("Jxara", new Town("Jxara",intializedGameData.colorsChosen.map(color => new TownPiece(color)),[]));
        // towns.set("Kihromah", new Town("Kihromah",intializedGameData.colorsChosen.map(color => new TownPiece(color)),[]));
        // towns.set("Lapphalya", new Town("Lapphalya",intializedGameData.colorsChosen.map(color => new TownPiece(color)),[]));
        // towns.set("Mah'Davikia", new Town("Mah'Davikia",intializedGameData.colorsChosen.map(color => new TownPiece(color)),[]));
        // towns.set("Parundia", new Town("Parundia",intializedGameData.colorsChosen.map(color => new TownPiece(color)),[]));
        // towns.set("Rivinia", new Town("Rivinia",intializedGameData.colorsChosen.map(color => new TownPiece(color)),[]));
        // towns.set("Strykhaven", new Town("Strykhaven",intializedGameData.colorsChosen.map(color => new TownPiece(color)),[]));
        // towns.set("Throtmanni", new Town("Throtmanni",intializedGameData.colorsChosen.map(color => new TownPiece(color)),[]));
        // towns.set("Tichih", new Town("Tichih",intializedGameData.colorsChosen.map(color => new TownPiece(color)),[]));
        // towns.set("Usselen", new Town("Usselen",intializedGameData.colorsChosen.map(color => new TownPiece(color)),[]));
        // towns.set("Virst", new Town("Virst",intializedGameData.colorsChosen.map(color => new TownPiece(color)),[]));
        // towns.set("Wylhien", new Town("Wylhien",intializedGameData.colorsChosen.map(color => new TownPiece(color)),[]));
        // towns.set("Yttar", new Town("Yttar",intializedGameData.colorsChosen.map(color => new TownPiece(color)),[]));

        // let roads: Road[] = [];
        // roads.push(new Road(towns["Usselen"], towns["Yttar"], RoadType.Forest));
        // roads.push(new Road(towns["Parundia"], towns["Usselen"], RoadType.Forest));
        // roads.push(new Road(towns["Parundia"], towns["Wylhien"], RoadType.Plain));
        // roads.push(new Road(towns["Parundia"], towns["Yttar"], RoadType.Lake));
        // roads.push(new Road(towns["Wylhien"], towns["Usselen"], RoadType.Stream));
        // roads.push(new Road(towns["Usselen"], towns["Wylhien"], RoadType.Plain));
        // roads.push(new Road(towns["Jaccaranda"], towns["Wylhien"], RoadType.Mountain));
        // roads.push(new Road(towns["Jaccaranda"], towns["Throtmanni"], RoadType.Mountain));
        // roads.push(new Road(towns["Jaccaranda"], towns["Tichih"], RoadType.Mountain));
        // roads.push(new Road(towns["Throtmanni"], towns["Tichih"], RoadType.Plain));
        // roads.push(new Road(towns["Erg'Eren"], towns["Tichih"], RoadType.Forest));
        // roads.push(new Road(towns["Rivinia"], towns["Tichih"], RoadType.Stream));
        // roads.push(new Road(towns["Rivinia"], towns["Throtmanni"], RoadType.Forest));
        // roads.push(new Road(towns["Elfenhold"], towns["Rivinia"], RoadType.Stream));
        // roads.push(new Road(towns["Elfenhold"], towns["Erg'Eren"], RoadType.Forest));
        // roads.push(new Road(towns["Beata"], towns["Elfenhold"], RoadType.Stream));
        // roads.push(new Road(towns["Beata"], towns["Elfenhold"], RoadType.Plain));
        // roads.push(new Road(towns["Beata"], towns["Strykhaven"], RoadType.Plain));
        // roads.push(new Road(towns["Strykhaven"], towns["Virst"], RoadType.Mountain));
        // roads.push(new Road(towns["Strykhaven"], towns["Virst"], RoadType.Lake));
        // roads.push(new Road(towns["Elfenhold"], towns["Strykhaven"], RoadType.Lake));
        // roads.push(new Road(towns["Elfenhold"], towns["Virst"], RoadType.Lake));
        // roads.push(new Road(towns["Elfenhold"], towns["Lapphalya"], RoadType.Plain));
        // roads.push(new Road(towns["Lapphalya"], towns["Rivinia"], RoadType.Forest));
        // roads.push(new Road(towns["Feodor"], towns["Lapphalya"], RoadType.Forest));
        // roads.push(new Road(towns["Feodor"], towns["Rivinia"], RoadType.Forest));
        // roads.push(new Road(towns["Feodor"], towns["Throtmanni"], RoadType.Desert));
        // roads.push(new Road(towns["Al'Baran"], towns["Throtmanni"], RoadType.Desert));
        // roads.push(new Road(towns["Al'Baran"], towns["Wylhien"], RoadType.Desert));
        // roads.push(new Road(towns["Al'Baran"], towns["Parundia"], RoadType.Desert));
        // roads.push(new Road(towns["Al'Baran"], towns["Feodor"], RoadType.Desert));
        // roads.push(new Road(towns["Dag'Amura"], towns["Feodor"], RoadType.Desert));
        // roads.push(new Road(towns["Dag'Amura"], towns["Kihromah"], RoadType.Forest));
        // roads.push(new Road(towns["Al'Baran"], towns["Dag'Amura"], RoadType.Desert));
        // roads.push(new Road(towns["Dag'Amura"], towns["Lapphalya"], RoadType.Forest));
        // roads.push(new Road(towns["Dag'Amura"], towns["Jxara"], RoadType.Forest));
        // roads.push(new Road(towns["Lapphalya"], towns["Jxara"], RoadType.Forest));
        // roads.push(new Road(towns["Virst"], towns["Jxara"], RoadType.Stream));
        // roads.push(new Road(towns["Jxara"], towns["Mah'Davikia"], RoadType.Stream));
        // roads.push(new Road(towns["Jxara"], towns["Mah'Davikia"], RoadType.Mountain));
        // roads.push(new Road(towns["Mah'Davikia"], towns["Grangor"], RoadType.Stream));
        // roads.push(new Road(towns["Dag'Amura"], towns["Mah'Davikia"], RoadType.Mountain));
        // roads.push(new Road(towns["Grangor"], towns["Mah'Davikia"], RoadType.Mountain));
        // roads.push(new Road(towns["Grangor"], towns["Yttar"], RoadType.Mountain));
        // roads.push(new Road(towns["Grangor"], towns["Parundia"], RoadType.Lake));
        // roads.push(new Road(towns["Grangor"], towns["Yttar"], RoadType.Lake));
        // roads.push(new Road(towns["Lapphalya"], towns["Virst"], RoadType.Plain));
        // roads.push(new Road(towns["Virst"], towns["Jxara"], RoadType.Plain));

    }

    [Test]
    public void UpdateTestsSimplePasses()
    {
        //Board board = new Board();
        //Game gameModel = new Game();
        //Assert.Equals(0, 0);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator UpdateTestsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
