
package javaproje;
import java.sql.*;
import java.util.logging.Level;
import java.util.logging.Logger;

public class Baglanti {

       static Connection con;
      static Statement mySt;
     
    
    public Baglanti() {
      
        
         try {
             con = (Connection)DriverManager.getConnection("jdbc:mysql://localhost:3306/javaproje?useUnicode=true&useLegacyDatetimeCode=false&serverTimezone=Turkey","root","1234");
          mySt = con.createStatement(); 
        } catch (SQLException ex) {
            Logger.getLogger(Baglanti.class.getName()).log(Level.SEVERE, null, ex);
        }
			               
            
    }
    
  
     public  static  ResultSet Baglan()  {
		 
    
         try {
      
            ResultSet myRs=mySt.executeQuery("select * from arac_bilgisi");
         
            
            return myRs;
         } catch (SQLException e) {
             System.out.println("baglanamadin");
              return null;
         }
		/* 
		 basliyacak yere yaz
		 baglanti bagYap = new baglanti();
		ResultSet Rs;
			Rs = bagYap.Baglan();
			 {
					try {
						while(Rs.next()) {
							System.out.println(Rs.getString("name"));
						}
					} catch (SQLException e) {
						// TODO Auto-generated catch block
						e.printStackTrace();
					}
			 }
		
		
		 
		 */
    

}
static boolean Ekle(String sql_sorgu){
           try {
               mySt.executeUpdate(sql_sorgu);
               return true;
           } catch (SQLException ex) {
               
               return false;
           }
        

}
static void Update(String sql_sorgu){

try {
               mySt.executeUpdate(sql_sorgu);
           } catch (SQLException ex) {
               Logger.getLogger(Baglanti.class.getName()).log(Level.SEVERE, null, ex);
           }


}
static void Sil(String sql_sorgu){
try {
               mySt.executeUpdate(sql_sorgu);
           } catch (SQLException ex) {
               Logger.getLogger(Baglanti.class.getName()).log(Level.SEVERE, null, ex);
           }

}
static ResultSet Sorgula(String sql_sorgu){

try {
      
            ResultSet myRs=mySt.executeQuery(sql_sorgu);
         
            
            return myRs;
         } catch (SQLException e) {
             System.out.println("baglanamadin sorgula");
              return null;
         }


}



}

