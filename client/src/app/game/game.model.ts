import { DbBoolean } from "../util/util";

export interface IGameDto {
  id: number;
  name: string;
  description: string | null;
  gameCategoryId: number;
  ageRatingId: number;
  releaseDate: string | null;
  coverImageUrl: string | null;
  isActive: DbBoolean;
}

export function createBlankGame(): IGameDto {
    return {
        id: 0,
        name: "",
        description: null,
        gameCategoryId: 0,
        ageRatingId: 0,
        releaseDate: null,
        coverImageUrl: null,
        isActive: DbBoolean.Yes
    };
}
