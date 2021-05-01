/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package javaproje;

import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.logging.Level;
import java.util.logging.Logger;
import static javaproje.FmLogin.KullaniciAdi;
import javax.swing.Timer;

import javax.swing.table.DefaultTableModel;


/**
 *
 * @author bari
 */
public class FmHome extends javax.swing.JFrame {

    
    /**
     * Creates new form home
     */
    
    
       static DefaultTableModel modelim =new DefaultTableModel();
       
   static Object[] satirlar=new Object[5];
   static Object[] kolonlar;
    static Object[] satirlarDefault={"",""};
    static String toplamAgirlik;
    
     static String kullanici=KullaniciAdi;
    
    public FmHome() {

        initComponents();

         
        txt_kullaniciAdi.setText(kullanici);
         Baglanti baglanti1;
        baglanti1 = new Baglanti();
        kolonlar = new Object[]{"Plaka", "Şöför","Giriş Tarihi","Kayit Eden","Ağirlik"};
    modelim.setColumnIdentifiers(kolonlar);  
    modelim.addRow(satirlarDefault);
    jTable1.setModel(modelim);
        Saat();
        

        agirlikhome.setText(String.valueOf(Agirlik())+" Kg");
        Listele();
        AracSayi();
        
        
    }
     public void Saat() { 
        Timer timer;
      
       
        ActionListener actionListener;
        actionListener = (ActionEvent e) -> {
            Date date =new Date();
            DateFormat timeFortmat = new SimpleDateFormat("HH:mm:ss");
            String time =timeFortmat.format(date);
            saat.setText(time);
            Date date1 =new Date();
            DateFormat timeFortmat1 = new SimpleDateFormat("dd.MM.yyyy");
            String time1 =timeFortmat1.format(date);
           tarih.setText(time1);
           
        };
        timer =new Timer(1000,actionListener);
        timer.setInitialDelay(0);
        timer.start();
      
    }
 public void AracSayi(){
 String sql_sorgu;
sql_sorgu="SELECT COUNT(plaka) FROM arac_bilgisi";
 ResultSet myRs=  Baglanti.Sorgula(sql_sorgu);
  try {
            while(myRs.next()){
           Lb_aracsayi.setText(myRs.getString("count(plaka)"));
            }
            
// TODO add your handling code here:
        } catch (SQLException ex) {
            Logger.getLogger(FmHome.class.getName()).log(Level.SEVERE, null, ex);
        }

 
 }
 public  static int Agirlik(){
 
   ResultSet myRs=  Baglanti.Baglan();
   
  int agirlik = 0;
   try {
            while(myRs.next()){
            
      agirlik+=(Integer.parseInt(myRs.getString("giris_agirligi")));
      
            } 
            return agirlik;
       //System.out.println(agirlik);       
// TODO add your handling code here:
        } catch (SQLException ex) {
            Logger.getLogger(FmHome.class.getName()).log(Level.SEVERE, null, ex);
        }
   
    return agirlik;
 
 }
     
     
public static DefaultTableModel Listele(){
     
  ResultSet myRs=  Baglanti.Baglan();
    kolonlar = new Object[]{"Plaka", "Şöför","Giriş Tarihi","Kayit Eden","Ağirlik(Kg)"};
 modelim.setColumnIdentifiers(kolonlar);      
  
 modelim.setRowCount(0);
  
   try {
            while(myRs.next()){
            satirlar[0]=(myRs.getString("plaka"));
            satirlar[1]=(myRs.getString("sofor"));
            satirlar[2]=(myRs.getString("giris_tarihi"));
            satirlar[3]=(myRs.getString("kayit_eden"));
            satirlar[4]=(myRs.getString("giris_agirligi"));
           
              
              
              modelim.addRow(satirlar);  
              
            }
            
// TODO add your handling code here:
        } catch (SQLException ex) {
            Logger.getLogger(FmHome.class.getName()).log(Level.SEVERE, null, ex);
        }
   
    jTable1.setModel(modelim);
    return modelim;
    }
    
   
    
    
    
    

    /**
     * This method is called from within the constructor to initialize the form.
     * WARNING: Do NOT modify this code. The content of this method is always
     * regenerated by the Form Editor.
     */
    @SuppressWarnings("unchecked")
    // <editor-fold defaultstate="collapsed" desc="Generated Code">//GEN-BEGIN:initComponents
    private void initComponents() {

        jButton1 = new javax.swing.JButton();
        jScrollPane1 = new javax.swing.JScrollPane();
        jTable1 = new javax.swing.JTable();
        jLabel1 = new javax.swing.JLabel();
        jLabel2 = new javax.swing.JLabel();
        txt_kullaniciAdi = new javax.swing.JLabel();
        Lb_aracsayi = new javax.swing.JLabel();
        saat = new javax.swing.JLabel();
        tarih = new javax.swing.JLabel();
        agirlikhome = new javax.swing.JLabel();
        jButton2 = new javax.swing.JButton();
        jLabel3 = new javax.swing.JLabel();
        jButton3 = new javax.swing.JButton();

        setDefaultCloseOperation(javax.swing.WindowConstants.EXIT_ON_CLOSE);
        setTitle("Home");

        jButton1.setBackground(new java.awt.Color(0, 0, 255));
        jButton1.setFont(new java.awt.Font("Tahoma", 1, 14)); // NOI18N
        jButton1.setForeground(new java.awt.Color(255, 255, 255));
        jButton1.setText("Giriş");
        jButton1.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButton1ActionPerformed(evt);
            }
        });

        jTable1.setModel(new javax.swing.table.DefaultTableModel(
            new Object [][] {
                {null, null, null, null},
                {null, null, null, null},
                {null, null, null, null},
                {null, null, null, null}
            },
            new String [] {
                "Title 1", "Title 2", "Title 3", "Title 4"
            }
        ));
        jScrollPane1.setViewportView(jTable1);

        jLabel1.setFont(new java.awt.Font("Tahoma", 1, 12)); // NOI18N
        jLabel1.setText("İçerde bulunan araç sayisi:");

        jLabel2.setFont(new java.awt.Font("Tahoma", 1, 12)); // NOI18N
        jLabel2.setText("Kullanici Adi:");

        txt_kullaniciAdi.setFont(new java.awt.Font("Tahoma", 1, 12)); // NOI18N
        txt_kullaniciAdi.setText("ad");

        Lb_aracsayi.setFont(new java.awt.Font("Tahoma", 1, 12)); // NOI18N
        Lb_aracsayi.setText("0");

        saat.setText("saat");

        tarih.setText("tarih");

        agirlikhome.setFont(new java.awt.Font("Tahoma", 1, 12)); // NOI18N
        agirlikhome.setForeground(new java.awt.Color(255, 255, 255));
        agirlikhome.setIcon(new javax.swing.ImageIcon(getClass().getResource("/javaproje/resim/iconAgirlikMor.png"))); // NOI18N
        agirlikhome.setText("0 Kg");
        agirlikhome.setHorizontalTextPosition(javax.swing.SwingConstants.CENTER);

        jButton2.setBackground(new java.awt.Color(0, 0, 255));
        jButton2.setFont(new java.awt.Font("Tahoma", 1, 14)); // NOI18N
        jButton2.setForeground(new java.awt.Color(255, 255, 255));
        jButton2.setText("Çıkış");
        jButton2.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButton2ActionPerformed(evt);
            }
        });

        jLabel3.setFont(new java.awt.Font("Tahoma", 1, 12)); // NOI18N
        jLabel3.setText("Toplam Agirlik:");

        jButton3.setIcon(new javax.swing.ImageIcon(getClass().getResource("/javaproje/resim/exit2.png"))); // NOI18N
        jButton3.setBorderPainted(false);
        jButton3.setContentAreaFilled(false);
        jButton3.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButton3ActionPerformed(evt);
            }
        });

        javax.swing.GroupLayout layout = new javax.swing.GroupLayout(getContentPane());
        getContentPane().setLayout(layout);
        layout.setHorizontalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(layout.createSequentialGroup()
                .addContainerGap()
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.TRAILING)
                    .addGroup(javax.swing.GroupLayout.Alignment.LEADING, layout.createSequentialGroup()
                        .addGap(305, 305, 305)
                        .addComponent(jLabel1)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addComponent(Lb_aracsayi, javax.swing.GroupLayout.PREFERRED_SIZE, 20, javax.swing.GroupLayout.PREFERRED_SIZE)
                        .addContainerGap(48, Short.MAX_VALUE))
                    .addGroup(layout.createSequentialGroup()
                        .addComponent(jScrollPane1, javax.swing.GroupLayout.PREFERRED_SIZE, 402, javax.swing.GroupLayout.PREFERRED_SIZE)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                        .addComponent(jButton3, javax.swing.GroupLayout.PREFERRED_SIZE, 50, javax.swing.GroupLayout.PREFERRED_SIZE))))
            .addGroup(layout.createSequentialGroup()
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(layout.createSequentialGroup()
                        .addGap(25, 25, 25)
                        .addComponent(jButton1)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addComponent(jButton2)
                        .addGap(20, 20, 20)
                        .addComponent(jLabel3)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.UNRELATED)
                        .addComponent(agirlikhome))
                    .addGroup(layout.createSequentialGroup()
                        .addGap(42, 42, 42)
                        .addComponent(jLabel2)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addComponent(txt_kullaniciAdi, javax.swing.GroupLayout.PREFERRED_SIZE, 86, javax.swing.GroupLayout.PREFERRED_SIZE)
                        .addGap(18, 18, 18)
                        .addComponent(tarih, javax.swing.GroupLayout.PREFERRED_SIZE, 51, javax.swing.GroupLayout.PREFERRED_SIZE)
                        .addGap(25, 25, 25)
                        .addComponent(saat, javax.swing.GroupLayout.PREFERRED_SIZE, 57, javax.swing.GroupLayout.PREFERRED_SIZE)))
                .addContainerGap(javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE))
        );
        layout.setVerticalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(layout.createSequentialGroup()
                .addContainerGap()
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(saat, javax.swing.GroupLayout.PREFERRED_SIZE, 15, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(tarih)
                    .addComponent(jLabel2)
                    .addComponent(txt_kullaniciAdi))
                .addGap(27, 27, 27)
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(layout.createSequentialGroup()
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED, 13, javax.swing.GroupLayout.PREFERRED_SIZE)
                        .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                            .addComponent(jButton1)
                            .addComponent(jButton2)))
                    .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                        .addComponent(jLabel3)
                        .addComponent(agirlikhome, javax.swing.GroupLayout.PREFERRED_SIZE, 42, javax.swing.GroupLayout.PREFERRED_SIZE)))
                .addGap(18, 18, 18)
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(jLabel1)
                    .addComponent(Lb_aracsayi))
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(layout.createSequentialGroup()
                        .addGap(26, 26, 26)
                        .addComponent(jScrollPane1, javax.swing.GroupLayout.PREFERRED_SIZE, 256, javax.swing.GroupLayout.PREFERRED_SIZE)
                        .addContainerGap(40, Short.MAX_VALUE))
                    .addGroup(javax.swing.GroupLayout.Alignment.TRAILING, layout.createSequentialGroup()
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                        .addComponent(jButton3, javax.swing.GroupLayout.PREFERRED_SIZE, 50, javax.swing.GroupLayout.PREFERRED_SIZE))))
        );

        pack();
    }// </editor-fold>//GEN-END:initComponents

    private void jButton1ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButton1ActionPerformed
        FmGiris fmGiris =new FmGiris();
        fmGiris.setVisible(true);


        // TODO add your handling code here:
    }//GEN-LAST:event_jButton1ActionPerformed

    private void jButton2ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButton2ActionPerformed
FmCikis Cikis=new FmCikis();
Cikis.setVisible(true);



        // TODO add your handling code here:
    }//GEN-LAST:event_jButton2ActionPerformed

    private void jButton3ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButton3ActionPerformed
 System.exit(0);
 // TODO add your handling code here:
    }//GEN-LAST:event_jButton3ActionPerformed

    
    
    /**
     * @param args the command line arguments
     */
    public static void main(String args[]) {
        /* Set the Nimbus look and feel */
        //<editor-fold defaultstate="collapsed" desc=" Look and feel setting code (optional) ">
        /* If Nimbus (introduced in Java SE 6) is not available, stay with the default look and feel.
         * For details see http://download.oracle.com/javase/tutorial/uiswing/lookandfeel/plaf.html 
         */
        try {
            for (javax.swing.UIManager.LookAndFeelInfo info : javax.swing.UIManager.getInstalledLookAndFeels()) {
                if ("Nimbus".equals(info.getName())) {
                    javax.swing.UIManager.setLookAndFeel(info.getClassName());
                    break;
                }
            }
        } catch (ClassNotFoundException ex) {
            java.util.logging.Logger.getLogger(FmHome.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        } catch (InstantiationException ex) {
            java.util.logging.Logger.getLogger(FmHome.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        } catch (IllegalAccessException ex) {
            java.util.logging.Logger.getLogger(FmHome.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        } catch (javax.swing.UnsupportedLookAndFeelException ex) {
            java.util.logging.Logger.getLogger(FmHome.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        }
        //</editor-fold>
        //</editor-fold>
        //</editor-fold>
        //</editor-fold>

        /* Create and display the form */
        java.awt.EventQueue.invokeLater(() -> {
            new FmHome().setVisible(true);
        });
    }

    // Variables declaration - do not modify//GEN-BEGIN:variables
    private javax.swing.JLabel Lb_aracsayi;
    public static javax.swing.JLabel agirlikhome;
    private javax.swing.JButton jButton1;
    private javax.swing.JButton jButton2;
    private javax.swing.JButton jButton3;
    private javax.swing.JLabel jLabel1;
    private javax.swing.JLabel jLabel2;
    private javax.swing.JLabel jLabel3;
    private javax.swing.JScrollPane jScrollPane1;
    private static javax.swing.JTable jTable1;
    private javax.swing.JLabel saat;
    private javax.swing.JLabel tarih;
    private javax.swing.JLabel txt_kullaniciAdi;
    // End of variables declaration//GEN-END:variables
}
