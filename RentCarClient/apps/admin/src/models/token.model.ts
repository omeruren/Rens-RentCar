export interface TokenModel {
  id: string;
  fullNameWithEmail: string;
  fullName: string;
  email: string;
  role: string;
  permissions: string[];
  branch: string;
}

export const INITIAL_TOKEN_MODEL: TokenModel = {
  id: '',
  fullNameWithEmail: '',
  fullName: '',
  email: '',
  role: '',
  permissions: [],
  branch: '',
};
