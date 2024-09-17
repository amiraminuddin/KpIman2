import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { Observable } from "rxjs";
import { environment } from "../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class ModuleService {

  constructor(private http: HttpClient, private route: Router) { }

  private url = environment.apiUrl;
  module: any;
  isActive: boolean = false;

  GetModuleList(): Observable<any[]> {
    return this.http.get<any[]>(this.url + "Module/GetAllModule");
  }

  UpdateModule(data: any) {
    return this.http.put(this.url + "Module/UpdateModule", data);
  }

  GetModuleById(Id: number) {
    const param = new HttpParams().set('Id', Id.toString());
    return this.http.get(`${this.url}Module/GetModuleById?${param}`);
  }

  GetModuleStatus(Id: number): Promise<boolean> {
    return new Promise((resolve) => {
      this.GetModuleById(Id).subscribe(
        (module: any) => {
          if (!module.isActive) {
            this.route.navigate(['kpIman/maintenance']);
          }
          resolve(module.isActive);
        }
      );
    });
  }

}
