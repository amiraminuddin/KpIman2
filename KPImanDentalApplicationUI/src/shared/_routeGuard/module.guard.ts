import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from "@angular/router";
import { Observable } from "rxjs";
import { ModuleService } from "../_services/module.service";


@Injectable({
  providedIn: 'root'
})
export class ModuleGuard implements CanActivate {

  constructor(private module: ModuleService, private router: Router) { }

  canActivate(): boolean {
    return true 
  }

}
