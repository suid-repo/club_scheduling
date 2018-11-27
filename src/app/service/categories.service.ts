import { Injectable } from '@angular/core';
import { FirebaseGenericService } from './firebase-generic.service';
import { ICategory } from '../interface/icategory';
import { AngularFirestore } from '@angular/fire/firestore';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class CategoriesService extends FirebaseGenericService<ICategory>
{

  constructor(protected _afs: AngularFirestore) 
  {
    super(_afs);
    this.initService('Categories');
  }

  getAll(): Observable<ICategory[]>
  {
    this.initCollection();

    return this.genericCollection.snapshotChanges().pipe
    (
      map(actions => actions.map(a => {
        const data = a.payload.doc.data() as ICategory;
        const id = a.payload.doc.id;

        return { id, ...data };
      }))
    );
  }
}
