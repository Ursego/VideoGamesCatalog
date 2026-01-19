import { CrudOperation, IDropdownEntry } from "../util/util";
import { IGameDto } from "./game.model";

export interface IGameState {
  gameList: IGameDto[];
  gameListLoaded: boolean;
  highlightedGame: IGameDto | null; // game clicked in Game List - to be picked by Edit & Delete buttons

  gameCategoryList: IDropdownEntry[];
  gameCategoryListLoaded: boolean;

  ageRatingList: IDropdownEntry[];
  ageRatingListLoaded: boolean;

  gameToSave: IGameDto | null;
  crudOperationToPerform: CrudOperation | null;
  gameLoaded: boolean;
}

export const initialGameState: IGameState = {
  gameList: [],
  gameListLoaded: false,
  highlightedGame: null,

  gameCategoryList: [],
  gameCategoryListLoaded: false,

  ageRatingList: [],
  ageRatingListLoaded: false,

  gameToSave: null,
  crudOperationToPerform: null,
  gameLoaded: false
};
