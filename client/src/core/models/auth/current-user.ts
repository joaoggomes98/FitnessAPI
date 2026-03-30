export interface CurrentUser {
  id: number;
  displayName: string;
  email: string;
  role: 'SuperAdmin' | 'PT' | 'Client';
  token: string;
}