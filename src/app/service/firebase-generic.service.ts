import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AngularFirestore, AngularFirestoreCollection, AngularFirestoreDocument } from '@angular/fire/firestore';

@Injectable({
  providedIn: 'root'
})
export class FirebaseGenericService<T> {

  genericCollection: AngularFirestoreCollection<T>;
  genericDocument: AngularFirestoreDocument<T>;
  
  _Name:string;

  constructor(protected _afs: AngularFirestore) { }

  initService(name:string):void
  {
    this._Name = name;
  }

  initCollection(): void
  {
    this.genericCollection = this._afs.collection<T>(this._Name);
  }

  initDocument(id:string): void
  {
    this.genericDocument = this._afs.doc<T>(this._Name+"/"+id);
  }

  get(id:string): Observable<T>
  {
    this.initDocument(id);

    return this.genericDocument.valueChanges();
  }

  getAll() : Observable<any[]>
  {
    // this.initCollection();

    // return this.genericCollection.snapshotChanges().pipe
    // (
    //   map(actions => actions.map(a => {
    //     const data = a.payload.doc.data() as any;
    //     const id = a.payload.doc.id;

    //     return { id, ...data };
    //   }))
    // );

    throw "getAll() method is not defined";
    
  }

  addR(customer: T):Promise<boolean>
  {
    this.initCollection();

    return this.genericCollection.add(customer)
    .catch(() => { return false;})
    .then(() => { return true; })
  }

  add(customer: T, id:string):void
  {
    this.initCollection();

    this.genericCollection.doc(id).set(customer);
  }

  update(id: string, customer: T): Promise<boolean>
  {
    this.initDocument(id);

    return this.genericDocument.update(customer).then()
    .catch(() => { return false;})
    .then(() => { return true; });
  }

  delete(id:string): Promise<boolean> 
  {
    this.initDocument(id);

    return this.genericDocument.delete()
    .catch(() => {return false})
    .then(() => {return true;});
  }
}
