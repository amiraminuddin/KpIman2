import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map } from "rxjs";
import { environment } from "../../environments/environment";


@Injectable({
  providedIn: 'root'
})

export class AccountServices {
  constructor(private http: HttpClient) { }

  private apiUrl = environment.apiUrl;
  private readonly TOKEN_KEY = 'authToken'; // Key for local storage
  userRegisterSuccess: boolean = false;
  userDeleteSuccess: boolean = false;
  isLogOut: boolean = false;


  login(userModel: any) {
    //return this.http.post(this.apiUrl + "Account/login", userModel).pipe(
    //  map(user => {
    //    if (user) {
    //      localStorage.setItem(this.TOKEN_KEY, JSON.stringify(user)); // Store token in local storage
    //    }
    //  }));
    return this.http.post(`${this.apiUrl}Account/login`, userModel);
  }

  register(userModel: any) {
    return this.http.post(this.apiUrl + "Account/register", userModel).pipe(
      map(user => {
        if (user) {
          this.userRegisterSuccess = true;
        }
        return this.userRegisterSuccess
      }));
  }

  deleteUser(id: number) {
    return this.http.delete(`${this.apiUrl}Account/deleteUser?Id=${id}`).pipe(
      map(result => {
        if (result) {
          this.userDeleteSuccess = true;
        }
        return this.userDeleteSuccess
      })
    )
  }

  logout() {
    localStorage.removeItem(this.TOKEN_KEY);
  }

  storeToken(token: string) {
    localStorage.setItem(this.TOKEN_KEY, JSON.stringify(token));
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem(this.TOKEN_KEY);
  }

  getToken(): string | null {
    // Retrieve the token data from localStorage
    const tokenData = localStorage.getItem(this.TOKEN_KEY);

    // Check if tokenData is not null
    if (tokenData) {
      try {
        // Parse the token data
        const parsedTokenData = JSON.parse(tokenData);
        // Return the userToken or null if it doesn't exist
        return parsedTokenData.userToken || null;
      } catch (error) {
        console.error('Error parsing token data:', error);
        return null; // Return null if there's an error parsing
      }
    }
    // Return null if no token data was found
    return null;
  }

  getCurrentUserLogin(): string | null {
    const tokenData = JSON.parse(localStorage.getItem(this.TOKEN_KEY) || 'null');
    return tokenData.userName;
  }

}
