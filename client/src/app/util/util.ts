export enum DbBoolean {
  Yes = "Y",
  No = "N"
}

export enum CrudOperation {
  Insert = "I",
  Update = "U",
  Delete = "D"
}

export interface IDropdownEntry {
  id: number;
  description: string;
}