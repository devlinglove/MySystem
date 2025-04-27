import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthenticationService } from '../authentication.service';
import { User } from '../../shared/models/account/user';
import { Router } from '@angular/router';
import { map, take } from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit{
  loginForm: FormGroup

  constructor(
      private fb: FormBuilder,
      private authService: AuthenticationService,
      private router: Router
    ){
    this.loginForm = this.fb.group({
      email: new FormControl('', {validators: [Validators.required]}),
      password: new FormControl('', {validators: [Validators.required]}),
    })
  }


  ngOnInit(): void {
    this.getCurrentUser()
   this.emailField.valueChanges.subscribe((value) => {
   })
  }


  get emailField(): FormControl {
		return this.loginForm.get( 'email' ) as FormControl;
	}

  get passwordField(): FormControl {
		return this.loginForm.get( 'password' ) as FormControl;
	}

  getCurrentUser(){
    this.authService.user$.pipe(
      take(1) 
    ).subscribe((user: User | null) => {      
      if(user?.token){
        this.router.navigate(['/dashboard'])
      }
    });
  }

  

  onSubmitForm(){
     console.warn(this.loginForm.value);
     this.authService.login(this.loginForm.value).subscribe({
      next: (response: User) => {
        console.log('response', response)
        if(response.token){
          this.router.navigate(['dashboard']);
        }
      }
     })
  }


}
