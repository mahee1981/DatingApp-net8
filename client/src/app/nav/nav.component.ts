import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountsService } from '../_services/accounts.service';
import {BsDropdownModule} from "ngx-bootstrap/dropdown"
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { TitleCasePipe } from '@angular/common';
import { UserLoginInfo } from '../_models/user';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule, BsDropdownModule, RouterLink, RouterLinkActive, TitleCasePipe],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
  model: UserLoginInfo = { username: "", password: "" };
  accountService = inject(AccountsService);
  private router = inject(Router);
  private toastr = inject(ToastrService);

  login() : void {
    this.accountService.login(this.model).subscribe({
      next: () => {
        this.router.navigateByUrl('/members');
        this.model.username = "";
        this.model.password = "";

      },
      error: error => {
        this.toastr.error(error.error);
      }
    });
  }

  logout() : void {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }
}
