import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map, Observable } from "rxjs";
import { environment } from "../../environments/environment";
import { DepartmentDto, PositionDto, UserDto } from "../model/AppModel";

@Injectable({
  providedIn : "root"
})
export class userServices {

  constructor(
    private http: HttpClient
  ) { }

  private apiUrl = environment.apiUrl;

  getUserById(id: number): Observable<UserDto>  {
    const param = new HttpParams().set('Id', id.toString());
    return this.http.get<UserDto>(`${this.apiUrl}Users/getUserById?${param}`).pipe();
  }


  updateUser(user: any) {
    return this.http.put(this.apiUrl + "Users/updateUser", user);
  }

  saveUserPhoto(file: any): Observable<string> {
    const formData = new FormData();
    formData.append('file', file);

    // Return an observable that emits the uploaded photo's URL
    return this.http.post<string>(this.apiUrl + 'Users/saveUserPhoto', formData);
  }

  checkUserChange(userInput: any) {
    return this.http.post(`${this.apiUrl}Users/checkUserChange`, userInput);
  }

  //START : Department
  getAllDepartment(): Observable<DepartmentDto[]> {
    return this.http.get<DepartmentDto[]>(this.apiUrl + "Users/getAllDepartment") 
  }

  adddepartment(departmentDto: any) {
    return this.http.post(`${this.apiUrl}Users/registerDeparment`, departmentDto).pipe(
      map(department => {
        if (department) {
          return true;
        }
        else {
          return false;
        }
      })
    );
  }

  //END : Department

  //START : Position
  getPositionByDepartment(department: string): Observable<PositionDto[]> {
    const params = `departmentCode=${department}`;
    return this.http.get<PositionDto[]>(`${this.apiUrl}Users/getPositionByDeprtmId?${params}`);
  }
  //END : Position
}
