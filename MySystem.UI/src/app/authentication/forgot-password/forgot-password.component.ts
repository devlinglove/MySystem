import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthenticationService } from '../authentication.service';
import { Router } from '@angular/router';
import { User } from '../../shared/models/account/user';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.scss'
})
export class ForgotPasswordComponent {

  forgotForm!: FormGroup
  
    constructor(
        private fb: FormBuilder,
        private authService: AuthenticationService,
        private router: Router
      ){
        
      this.forgotForm = this.fb.group({
        email: new FormControl({value: '', disabled: false}, {validators: [Validators.required]}),
      })
    }

    onSubmitForm(){
      this.authService.forgot(this.forgotForm.value).subscribe({
        next: (response: User | null) => {
          if(response){
            this.router.navigate(['account/reset-password'], {
              queryParams: {
                email: response.email
              }
            })
          }
        }
      })
    }


}
