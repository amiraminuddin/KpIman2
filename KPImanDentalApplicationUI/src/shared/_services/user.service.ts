import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map, Observable } from "rxjs";
import { environment } from "../../environments/environment";
import { ActionValidatorsInput, ActionValidatorsOutput, DataValidator, DeletionCondition, DepartmentDto, PositionDto, UserDto, Validators } from "../model/AppModel";

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
    return this.http.put(this.apiUrl + "Users/updateUser", user).pipe();
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
  createOrUpdateDepartment(departmentDto: DepartmentDto) {
    return this.http.post(`${this.apiUrl}Users/CreateOrUpdateDepartment`, departmentDto).pipe();
  }

  getAllDepartment(): Observable<DepartmentDto[]> {
    return this.http.get<DepartmentDto[]>(this.apiUrl + "Users/GetAllDepartment").pipe();
  }

  getDepartmentById(id: number): Observable<DepartmentDto> {
    const params = `id=${id}`;
    return this.http.get<DepartmentDto>(`${this.apiUrl}Users/GetDepartmentById?${params}`).pipe();
    //return this.http.get<TreatmeantLookupDto>(`${this.apiUrl}Lookup/GetTreatmentById?Id=${id}`).pipe();
  }

  deleteDepartment(id: number) {
    const params = `id=${id}`;
    return this.http.delete(`${this.apiUrl}/DeleteDepartment?${params}`).pipe();
  }

  checkPositionExistByDepartment(id: number) {
    const params = `id=${id}`;
    return this.http.get(`${this.apiUrl}/CheckPositionExistByDepartment?${params}`).pipe();
  }

  canDeleteDepartment<T>(id: number): Observable<DeletionCondition<T>> {
    const params = `DepartmentId=${id}`;
    return this.http.get<DeletionCondition<T>>(`${this.apiUrl}Users/CanDeleteDepartment?${params}`).pipe();
  }

  getDepartmentActionValidator(request: ActionValidatorsInput<DepartmentDto>): Observable<ActionValidatorsOutput[]> {
    return this.http.post<ActionValidatorsOutput[]>(`${this.apiUrl}Users/GetDepartmentActionValidator`, request).pipe();
  }

  getDepartmentValidator(departmentDto: DataValidator<DepartmentDto>): Observable<Validators[]> {
    return this.http.post<Validators[]>(`${this.apiUrl}Users/GetDepartmentValidator`, departmentDto).pipe();
  }
  //END : Department

  //START : Position
  getPositionByDeprtmId(department: number): Observable<PositionDto[]> {
    const params = `departmentId=${department}`;
    return this.http.get<PositionDto[]>(`${this.apiUrl}Users/GetPositionByDeprtmId?${params}`).pipe();
  }

  createOrUpdatePosition(positionDto: PositionDto) {
    return this.http.post(`${this.apiUrl}Users/CreateOrUpdatePosition`, positionDto).pipe();
  }

  getPositionById(id: number): Observable<PositionDto> {
    const params = `id=${id}`;
    return this.http.get<PositionDto>(`${this.apiUrl}Users/GetPositionById?${params}`).pipe();
  }

  deletePosition(id: number) {
    const params = `id=${id}`;
    return this.http.delete(`${this.apiUrl}DeletePosition?${params}`).pipe();
  }

  canDeletePosition<T>(id: number): Observable<DeletionCondition<T>> {
    const params = `id=${id}`;
    return this.http.get<DeletionCondition<T>>(`${this.apiUrl}Users/CanDeletePosition?${params}`).pipe();
  }
  //END : Position
}
