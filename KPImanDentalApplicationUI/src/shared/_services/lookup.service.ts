import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "../../environments/environment";
import { StaffLookupDto, TreatmeantLookupDto } from "../model/AppModel";

@Injectable({
  providedIn : 'root'
})

export class LookupService {

  constructor(private http: HttpClient) { }

  private apiUrl = environment.apiUrl;

  //START : Treatment Lookup
  createOrUpdateTreatmentLookup(data: TreatmeantLookupDto) {
    return this.http.post(this.apiUrl + "Lookup/CreateOrUpdateTreatmentLookup", data).pipe();
  }

  getAllTreatment(): Observable<TreatmeantLookupDto[]> {
    return this.http.get<TreatmeantLookupDto[]>(this.apiUrl + "Lookup/GetAllTreatment").pipe();
  }

  getTreatmentById(id: number): Observable<TreatmeantLookupDto> {
    return this.http.get<TreatmeantLookupDto>(`${this.apiUrl}Lookup/GetTreatmentById?Id=${id}`).pipe();
  }

  deleteTreatment(id: number) {
    return this.http.delete(`${this.apiUrl}Lookup/DeleteTreatment?Id=${id}`).pipe();
  }
  //END : Treatment Lookup

  //START : User Lookup
  getUserLookup(position: string): Observable<StaffLookupDto[]> {
    return this.http.get<StaffLookupDto[]>(`${this.apiUrl}Lookup/GetUserLookup?position=${position}`).pipe();
  }

  getUserLookupByHierachyLevel(hierachyLevel: number): Observable<StaffLookupDto[]> {
    return this.http.get<StaffLookupDto[]>(`${this.apiUrl}Lookup/GetUserLookupByHierachyLevel?hierachyLevel=${hierachyLevel}`).pipe();
  }
  //END : User Lookup
}
