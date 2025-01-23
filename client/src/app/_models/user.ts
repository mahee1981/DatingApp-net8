export interface User {
    username: string;
    token: string;
    photoUrl?: string;
    knownAs: string;
    gender: string;
}

export interface UserLoginInfo {
    username: string;
    password: string;
}