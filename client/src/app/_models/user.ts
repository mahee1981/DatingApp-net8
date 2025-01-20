export interface User {
    username: string;
    token: string;
    photoUrl?: string;
}

export interface UserLoginInfo {
    username: string;
    password: string;
}