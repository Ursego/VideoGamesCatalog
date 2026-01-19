/*
# Preparation steps (ONE TIME):
cd "<YOUR PATH>/video-game-catalog/server/"
dotnet new xunit -n VideoGameCatalog.Api.Tests
dotnet sln add VideoGameCatalog.Api.Tests
cd VideoGameCatalog.Api.Tests
dotnet add package Microsoft.NET.Test.Sdk
dotnet add package Moq # For mocking GameBl
dotnet add package Microsoft.AspNetCore.Mvc.Testing # Optional: integration tests
dotnet add reference ../VideoGameCatalog.Api/ # Reference main project

# Run the tests:
cd "<YOUR PATH>/video-game-catalog/server/"
dotnet test
*/

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VideoGameCatalog.Api.Modules.Game;
using VideoGameCatalog.Api.Modules.Game.Dtos;
using VideoGameCatalog.Api.Modules.Game.EfEntities;
using Xunit;

namespace VideoGameCatalog.Api.Tests;

public class GameControllerTests
{
	// Helper: create controller wired to a real BL, backed by a mocked DB.
	private static (Mock<IGameDb> mockDb, GameController controller) CreateController()
	{
		var mockDb = new Mock<IGameDb>(MockBehavior.Strict);
		var bl = new GameBl(mockDb.Object);
		var controller = new GameController(bl);
		return (mockDb, controller);
	}

	[Fact]
	public async Task SelectGameList_WithName_ReturnsList()
	{
		var (mockDb, controller) = CreateController();

		var entityList = new List<Game>
		{
			new Game { Id = 1, Name = "Test Game", GameCategoryId = 1, AgeRatingId = 1 }
		};

		mockDb
			.Setup(x => x.SelectGameListAsync("Test", It.IsAny<CancellationToken>()))
			.ReturnsAsync(entityList);

		var result = await controller.SelectGameList("Test", CancellationToken.None);

		Assert.Single(result);
		Assert.Equal(1, result[0].Id);
		Assert.Equal("Test Game", result[0].Name);

		mockDb.Verify(x => x.SelectGameListAsync("Test", It.IsAny<CancellationToken>()), Times.Once);
		mockDb.VerifyNoOtherCalls();
	}

	[Fact]
	public async Task InsertGame_ValidDto_ReturnsCreated()
	{
		var (mockDb, controller) = CreateController();

		var dto = new GameDto { Id = 123, Name = "New Game", GameCategoryId = 1, AgeRatingId = 1 };

		// BL sets entity.Id = 0 before insert; DB returns saved entity with generated Id.
		mockDb
			.Setup(x => x.InsertGameAsync(It.Is<Game>(g =>
				g.Id == 0 &&
				g.Name == "New Game" &&
				g.GameCategoryId == 1 &&
				g.AgeRatingId == 1
			), It.IsAny<CancellationToken>()))
			.ReturnsAsync(new Game { Id = 1, Name = "New Game", GameCategoryId = 1, AgeRatingId = 1 });

		var result = await controller.InsertGame(dto, CancellationToken.None);

		var createdResult = Assert.IsType<CreatedResult>(result.Result);
		var returnedGame = Assert.IsType<GameDto>(createdResult.Value);

		Assert.Equal(1, returnedGame.Id);
		Assert.Equal("New Game", returnedGame.Name);
		Assert.Equal("/games/1", createdResult.Location);

		mockDb.VerifyAll();
		mockDb.VerifyNoOtherCalls();
	}

	[Fact]
	public async Task UpdateGame_ValidIdAndDto_ReturnsOk()
	{
		var (mockDb, controller) = CreateController();

		var dto = new GameDto { Id = 1, Name = "Updated", GameCategoryId = 1, AgeRatingId = 1 };

		mockDb
			.Setup(x => x.UpdateGameAsync(It.Is<Game>(g =>
				g.Id == 1 &&
				g.Name == "Updated" &&
				g.GameCategoryId == 1 &&
				g.AgeRatingId == 1
			), It.IsAny<CancellationToken>()))
			.ReturnsAsync(new Game { Id = 1, Name = "Updated", GameCategoryId = 1, AgeRatingId = 1 });

		var result = await controller.UpdateGame(1, dto, CancellationToken.None);

		var okResult = Assert.IsType<OkObjectResult>(result.Result);
		var returnedGame = Assert.IsType<GameDto>(okResult.Value);

		Assert.Equal(1, returnedGame.Id);
		Assert.Equal("Updated", returnedGame.Name);

		mockDb.VerifyAll();
		mockDb.VerifyNoOtherCalls();
	}

	[Fact]
	public async Task UpdateGame_NotFound_ReturnsNotFound()
	{
		var (mockDb, controller) = CreateController();

		var dto = new GameDto { Id = 1, Name = "DoesNotMatter", GameCategoryId = 1, AgeRatingId = 1 };

		mockDb
			.Setup(x => x.UpdateGameAsync(It.Is<Game>(g => g.Id == 1), It.IsAny<CancellationToken>()))
			.ThrowsAsync(new KeyNotFoundException());

		var result = await controller.UpdateGame(1, dto, CancellationToken.None);

		Assert.IsType<NotFoundResult>(result.Result);

		mockDb.VerifyAll();
		mockDb.VerifyNoOtherCalls();
	}

	[Fact]
	public async Task DeleteGame_ValidId_ReturnsNoContent()
	{
		var (mockDb, controller) = CreateController();

		mockDb
			.Setup(x => x.DeleteGameAsync(1, It.IsAny<CancellationToken>()))
			.Returns(Task.CompletedTask);

		var result = await controller.DeleteGame(1, CancellationToken.None);

		Assert.IsType<NoContentResult>(result);

		mockDb.VerifyAll();
		mockDb.VerifyNoOtherCalls();
	}

	[Fact]
	public async Task SelectGameCategoryList_ReturnsList()
	{
		var (mockDb, controller) = CreateController();

		mockDb
			.Setup(x => x.SelectGameCategoryListAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(new List<GameCategory> { new GameCategory { Id = 1, Description = "Action" } });

		var result = await controller.SelectGameCategoryList(CancellationToken.None);

		Assert.Single(result);
		Assert.Equal(1, result[0].Id);
		Assert.Equal("Action", result[0].Description);

		mockDb.VerifyAll();
		mockDb.VerifyNoOtherCalls();
	}

	[Fact]
	public async Task SelectAgeRatingList_ReturnsList()
	{
		var (mockDb, controller) = CreateController();

		mockDb
			.Setup(x => x.SelectAgeRatingListAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(new List<AgeRating> { new AgeRating { Id = 1, Description = "E" } });

		var result = await controller.SelectAgeRatingList(CancellationToken.None);

		Assert.Single(result);
		Assert.Equal(1, result[0].Id);
		Assert.Equal("E", result[0].Description);

		mockDb.VerifyAll();
		mockDb.VerifyNoOtherCalls();
	}
}
