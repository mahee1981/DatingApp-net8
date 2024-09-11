import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { User } from '../_models/user';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
// these are singletons
export class AccountsService {

  private http = inject(HttpClient);
  baseUrl = 'https://localhost:5001/api/';
  currentUser = signal<User | null>(null);

  sendUserInformation(model: any, route: string) {
    return this.http.post<User>(this.baseUrl + 'account/' + route, model).pipe(
      map(user => {
        if(user){
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUser.set(user);
        }
        return user;
      })
    );
  }

  login(model: any) {
    return this.sendUserInformation(model, 'login');
  }

  
  register(model: any) {
    return this.sendUserInformation(model, 'register');
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUser.set(null);
  }
}
