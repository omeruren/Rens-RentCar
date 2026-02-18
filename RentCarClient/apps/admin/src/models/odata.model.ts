export interface ODataModel<T> {
  value: T[];
  ['@count.count']: number;
}
