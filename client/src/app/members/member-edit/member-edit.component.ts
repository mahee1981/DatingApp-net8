import { Component, HostListener, inject, input, OnInit, ViewChild } from '@angular/core';
import { Member } from '../../_models/member';
import { AccountsService } from '../../_services/accounts.service';
import { MembersService } from '../../_services/members.service';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { FormsModule, NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { PhotoEditorComponent } from "../photo-editor/photo-editor.component";

@Component({
  selector: 'app-member-edit',
  standalone: true,
  imports: [TabsModule, FormsModule, PhotoEditorComponent],
  templateUrl: './member-edit.component.html',
  styleUrl: './member-edit.component.css'
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm?: NgForm
  @HostListener("window:beforeunload", ['$event']) notify($event:any) {
    if(this.editForm?.dirty){
      $event.returnValue = true;
    }
  } 
  
  member?: Member;
  private accountService = inject(AccountsService);
  private memberService = inject(MembersService);
  private toastr = inject(ToastrService);

  ngOnInit(): void {
      this.loadMember();
  }

  loadMember() : void {
    const username = this.accountService.currentUser()?.username;
    if(!username)
      return;
      this.memberService.getMember(username).subscribe({
        next: (result: Member) => this.member = result,
      }
      );
  }

  updateMember() : void {
    this.memberService.updateMember(this.editForm?.value).subscribe({
      next: _ => {
        this.toastr.success("Profile updated successfully!");
        this.editForm?.reset(this.member)
      }
    });
  }

  onMemberChange(event: Member) : void {
    this.member = event; 
  }

}
