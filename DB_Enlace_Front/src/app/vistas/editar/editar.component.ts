import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { IEncargado } from '../../modelos/encargado.interfase';
import { ApiService } from '../../Servicios/api/api.service';
import { FormGroup,FormControl,Validators } from '@angular/forms';


@Component({
  selector: 'app-editar',
  templateUrl: './editar.component.html',
  styleUrl: './editar.component.css'
})

export class EditarComponent implements OnInit{

  constructor(private activerouter:ActivatedRoute, private router:Router, private api:ApiService){ }

  datosEncargado:IEncargado | undefined;

  editarForm = new FormGroup({
    nombre: new FormControl(''),
    apellido: new FormControl(''),
    direccion: new FormControl(''),
    email: new FormControl(''),
    telefono: new FormControl(''),
    token: new FormControl(''),
    encargadoID: new FormControl('')
});

  ngOnInit(): void {
    let encargadoId = this.activerouter.snapshot.paramMap.get('id');
    let token = this.getToken();
    this.api.getSingleEncargado(encargadoId).subscribe(data => {
        this.datosEncargado = data;
        this.editarForm.setValue({
          'nombre': this.datosEncargado.nombre,
          'apellido': this.datosEncargado.apellido,
          'direccion': this.datosEncargado.direccion,
          'email': this.datosEncargado.email,
          'telefono': this.datosEncargado.telefono,
          'token': token,
          'encargadoID': encargadoId
        })
    })
  }

  getToken(){
    return localStorage.getItem('token');
  }

}
